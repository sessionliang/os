/// <reference path="siteserver.cms.ts"/>

/**
*
*   This is base interface for siteserver ts platform.
*   All methods and properties are looking for in siteserver.js.
*
**/
interface ISiteServerExtendJs {
    //TODO: siteserver.extend.js 中抽象接口。
    //校验对象
    validater: Object;
    //检测表单
    checkFormValueById(formId: string): boolean;
    //blur检测
    event_observe(attributeName: string, eventName: string, handler: string): void;
}

declare module "ss.extendjs" {
    export = ss.extendjs;
}

//如果已经有javascript文件实现了ts接口，那么只需要定义一个接口类型的变量即可。
declare var extendjs: ISiteServerExtendJs;
