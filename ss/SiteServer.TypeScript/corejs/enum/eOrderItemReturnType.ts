enum eOrderItemReturnType {
    Return, //退货
    Exchange, //换货
    Repair  // 维修
}

class eOrderItemReturnTypeUtils {
    static getValue(type: eOrderItemReturnType): string {
        if (type === eOrderItemReturnType.Return) {
            return "Return";
        } else if (type === eOrderItemReturnType.Exchange) {
            return "Exchange";
        } else if (type === eOrderItemReturnType.Repair) {
            return "Exchange";
        }
        return StringUtils.empty;
    }

    static getText(type: eOrderItemReturnType): string {
        if (type === eOrderItemReturnType.Return) {
            return "退货";
        } else if (type === eOrderItemReturnType.Exchange) {
            return "换货";
        } else if (type === eOrderItemReturnType.Repair) {
            return "维修";
        }
        return StringUtils.empty;
    }

    static getEnum(type: string): eOrderItemReturnType {
        if (eOrderItemReturnTypeUtils.equals(type, eOrderItemReturnType.Return)) {
            return eOrderItemReturnType.Return;
        } else if (eOrderItemReturnTypeUtils.equals(type, eOrderItemReturnType.Exchange)) {
            return eOrderItemReturnType.Exchange;
        } else if (eOrderItemReturnTypeUtils.equals(type, eOrderItemReturnType.Repair)) {
            return eOrderItemReturnType.Repair;
        }
        return eOrderItemReturnType.Return;
    }

    static equals(typeStr: string, type: eOrderItemReturnType): boolean {
        if (typeStr === eOrderItemReturnTypeUtils.getValue(type)) {
            return true;
        }
        else
            return false;
    }

    static getAllItem(): _map {
        var m = new _map();
        m.set(eOrderItemReturnTypeUtils.getValue(eOrderItemReturnType.Return), eOrderItemReturnTypeUtils.getText(eOrderItemReturnType.Return));
        return m;
    }

    static addItemsToEle(ele: HTMLElement, childEleStr: string, childEleClass?: string, otherAttrs?: _map) {
        if (ele !== null) {
            if (childEleStr === "")
                childEleStr = "option";
            var items = eOrderItemReturnTypeUtils.getAllItem();
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

/**
* 自定义map集合
* ES5中没有map类型
*/
class _map {
    private dict = Object.create(null);
    get(key) {
        return this.dict[key];
    }
    set(value, key) {
        this.dict[key] = value;
    }
    forEach: any = function (func: Function) {
        for (var key in this.dict.keys) {
            func(this.get(key), key, this.dict);
        }
    };
}