enum eInputType {
    CheckBox,
    Radio,
    SelectOne,
    SelectMultiple,
    Date,
    DateTime,
    Image,
    Video,
    File,
    Text,
    TextArea,
    TextEditor,
    RelatedField,
    SpecifiedValue
}

class eInputTypeUtils {
    static getValue(type: eInputType): string {
        if (type === eInputType.CheckBox) {
            return "CheckBox";
        } else if (type === eInputType.Radio) {
            return "Radio";
        } else if (type === eInputType.SelectOne) {
            return "SelectOne";
        } else if (type === eInputType.SelectMultiple) {
            return "SelectMultiple";
        } else if (type === eInputType.Date) {
            return "Date";
        } else if (type === eInputType.DateTime) {
            return "DateTime";
        } else if (type === eInputType.Image) {
            return "Image";
        } else if (type === eInputType.Video) {
            return "Video";
        } else if (type === eInputType.File) {
            return "File";
        } else if (type === eInputType.Text) {
            return "Text";
        } else if (type === eInputType.TextArea) {
            return "TextArea";
        } else if (type === eInputType.TextEditor) {
            return "TextEditor";
        } else if (type === eInputType.RelatedField) {
            return "RelatedField";
        } else if (type === eInputType.SpecifiedValue) {
            return "SpecifiedValue";
        }
        return StringUtils.empty;
    }

    static getText(type: eInputType): string {
        if (type === eInputType.CheckBox) {
            return "复选列表(checkbox)";
        } else if (type === eInputType.Radio) {
            return "单选列表(radio)";
        } else if (type === eInputType.SelectOne) {
            return "下拉列表(select单选)";
        } else if (type === eInputType.SelectMultiple) {
            return "下拉列表(select多选)";
        } else if (type === eInputType.Date) {
            return "日期选择框";
        } else if (type === eInputType.DateTime) {
            return "日期时间选择框";
        } else if (type === eInputType.Image) {
            return "图片";
        } else if (type === eInputType.Video) {
            return "视频";
        } else if (type === eInputType.File) {
            return "附件";
        } else if (type === eInputType.Text) {
            return "文本框(单行)";
        } else if (type === eInputType.TextArea) {
            return "文本框(多行)";
        } else if (type === eInputType.TextEditor) {
            return "内容编辑器";
        } else if (type === eInputType.RelatedField) {
            return "联动字段";
        } else if (type === eInputType.SpecifiedValue) {
            return "系统指定值";
        }
        return StringUtils.empty;
    }

    static getEnum(type: string): eInputType {
        if (eInputTypeUtils.equals(type, eInputType.CheckBox)) {
            return eInputType.CheckBox;
        } else if (eInputTypeUtils.equals(type, eInputType.Radio)) {
            return eInputType.Radio;
        } else if (eInputTypeUtils.equals(type, eInputType.SelectOne)) {
            return eInputType.SelectOne;
        } else if (eInputTypeUtils.equals(type, eInputType.SelectMultiple)) {
            return eInputType.SelectMultiple;
        } else if (eInputTypeUtils.equals(type, eInputType.Date)) {
            return eInputType.Date;
        } else if (eInputTypeUtils.equals(type, eInputType.DateTime)) {
            return eInputType.DateTime;
        } else if (eInputTypeUtils.equals(type, eInputType.Image)) {
            return eInputType.Image;
        } else if (eInputTypeUtils.equals(type, eInputType.Video)) {
            return eInputType.Video;
        } else if (eInputTypeUtils.equals(type, eInputType.File)) {
            return eInputType.File;
        } else if (eInputTypeUtils.equals(type, eInputType.Text)) {
            return eInputType.Text;
        } else if (eInputTypeUtils.equals(type, eInputType.TextArea)) {
            return eInputType.TextArea;
        } else if (eInputTypeUtils.equals(type, eInputType.TextEditor)) {
            return eInputType.TextEditor;
        } else if (eInputTypeUtils.equals(type, eInputType.RelatedField)) {
            return eInputType.RelatedField;
        } else if (eInputTypeUtils.equals(type, eInputType.SpecifiedValue)) {
            return eInputType.SpecifiedValue;
        }
        return eInputType.Text;
    }

    static equals(typeStr: string, type: eInputType): boolean {
        if (typeStr === eInputTypeUtils.getValue(type)) {
            return true;
        }
        else
            return false;
    }

    static getAllItem(): _map {
        var m = new _map();
        m.set(eInputTypeUtils.getValue(eInputType.CheckBox), eInputTypeUtils.getText(eInputType.CheckBox));
        return m;
    }

    static addItemsToEle(ele: HTMLElement, childEleStr: string, childEleClass?: string, otherAttrs?: _map) {
        if (ele !== null) {
            if (childEleStr === "")
                childEleStr = "option";
            var items = eInputTypeUtils.getAllItem();
            items.forEach((val, key, map) => {
                var childEle;
                if (childEleStr === "option") {
                    childEle = document.createElement(childEleStr);
                    childEle.setAttribute("value", key);
                    childEle.innerHTML = val;
                    ele.appendChild(childEle);
                }
                else if (childEleStr === "radio") {
                    childEle = document.createElement("input");
                    childEle.setAttribute("type", "radio");
                    childEle.setAttribute("value", key);
                    var labelEle = document.createElement("label");
                    labelEle.innerHTML = val;
                    ele.appendChild(labelEle);
                    ele.insertBefore(childEle, labelEle);
                }
                else if (childEleStr === "checkbox") {
                    childEle = document.createElement("input");
                    childEle.setAttribute("type", "checkbox");
                    childEle.setAttribute("value", key);
                    var labelEle = document.createElement("label");
                    labelEle.innerHTML = val;
                    ele.appendChild(labelEle);
                    ele.insertBefore(childEle, labelEle);
                }
                if (childEleClass) {
                    childEle.className = childEleClass;
                }
                if (otherAttrs) {
                    otherAttrs.forEach((val, key, map) => {
                        childEle.setAttribute(key, val);
                    });
                }
            });
        }
    }
}