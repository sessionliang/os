/// <reference path="siteserver.ts"/>
/// <reference path="core/tablestyleutils.ts" />

/**
*
*   siteserver cms ts file
*   this is a demo!!!
*
**/
interface ISiteServerCMS {

    //TODO: 添加方法，需要在SiteServerCMS中实现。

    //解析表单元素，根据TableStyleInfoList
    parserForm(formCollection: _map, styleInfoArray: Object[], isEdit?: boolean, isValidate?: boolean, pannel?: HTMLElement): string;
}

class SiteServerCMS implements ISiteServerCMS {
    constructor() { }

    parserForm(formCollection: _map, styleInfoArray: Object[], isEdit?: boolean, isValidate?: boolean, pannel?: HTMLElement): string {
        return TableStyleUtils.render(formCollection, styleInfoArray, isEdit, isValidate, pannel);
    }
}

declare module "ss.cms" {
    export = ss.cms;
}

ss.cms = new SiteServerCMS();
