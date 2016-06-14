var gulp = require('gulp');
var ftp = require('gulp-ftp');
var rimraf = require('rimraf');
var replace = require('gulp-replace');
var fs = require('fs');
var mime = require('mime');
var aws = require('aws-sdk');
var zip = require('gulp-zip');
var args = require('yargs').argv;
var msbuild = require('gulp-msbuild');
var runSequence = require('run-sequence').use(gulp);

gulp.task('delete', function (cb) {
    return rimraf('./_publish', cb);
});

gulp.task('build', function () {
    return gulp
        .src('./ALL.sln')
        .pipe(msbuild({
            toolsVersion: 14.0,
            targets: ['Clean', 'Build'],
            errorOnFail: true,
            stdout: false
        }));
});

gulp.task('copy_SiteFiles', function () {
    gulp.src(['./Web/SiteFiles/bairong/**'])
        .pipe(gulp.dest('./_publish/SiteFiles/bairong'));
    gulp.src(['./Web/SiteFiles/Configuration/**'])
        .pipe(gulp.dest('./_publish/SiteFiles/Configuration'));
    gulp.src(['./Web/SiteFiles/Products/**'])
        .pipe(gulp.dest('./_publish/SiteFiles/Products'));
    gulp.src(['./Web/SiteFiles/Services/**'])
        .pipe(gulp.dest('./_publish/SiteFiles/Services'));
    return gulp.src(['./Web/SiteFiles/index.htm'])
        .pipe(gulp.dest('./_publish/SiteFiles'));
});

gulp.task('copy_SiteServer', function () {
    return gulp.src(['./Web/SiteServer/**'])
        .pipe(gulp.dest('./_publish/SiteServer'));
});

gulp.task('copy_bin', function () {
    return gulp.src(['./web/bin/**.dll'])
        .pipe(gulp.dest('./_publish/bin'))
});

gulp.task('copy_others', function () {
    return gulp.src(['./web/web.config', './web/version.txt'])
        .pipe(gulp.dest('./_publish'))
});

gulp.task('publish', function (callback) {
    runSequence('delete', 'build', 'copy_SiteFiles', 'copy_SiteServer', 'copy_bin', 'copy_others');
});