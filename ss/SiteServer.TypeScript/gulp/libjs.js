var gulp = require('gulp');
var watch = require('gulp-watch');
var concat = require('gulp-concat');
var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var tmodjs = require('gulp-tmod');
var cssmin = require('gulp-cssmin');
var less = require('gulp-less');

gulp.task('libjs', function () {
    return gulp.src([
        './assets/lib/jquery/1.11.1/jquery.js',
        './assets/lib/jquery.cookie/1.4.1/jquery.cookie.js',
        './assets/lib/jquery.fileupload/5.42.0/jquery.ui.widget.js',
        './assets/lib/jquery.fileupload/5.42.0/jquery.fileupload.js',
        './assets/lib/jquery.fileupload/5.42.0/jquery.iframe-transport.js',
        './assets/lib/siteserver/siteserver.extend.js',
        './assets/lib/siteserver/siteserver.validate.js'])
        .pipe(concat('lib.js'))
        //.pipe(uglify())
        .pipe(gulp.dest('./assets/js'))
});