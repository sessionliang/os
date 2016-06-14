/**
*
*   This is base interface for siteserver ts platform.
*   All methods and properties are looking for in siteserver.js.
*
**/
interface ISiteServer {
    cms: ISiteServerCMS, //cms js lib
    extendjs: ISiteServerExtendJs, //extend javascript lib
    test(): void;
}

class SiteServer implements ISiteServer {
    cms: ISiteServerCMS;
    extendjs: ISiteServerExtendJs;
    test(): void {
        alert("siteserver.test call");
    }
}

declare module "ss" {
    export = ss;
}

//如果没有javascript文件实现，那么需要在最后实例化ts对象
declare var ss: ISiteServer;



