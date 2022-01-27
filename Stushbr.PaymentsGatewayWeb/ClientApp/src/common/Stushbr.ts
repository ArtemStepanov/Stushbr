import {ErrorsResponse} from "../models/ErrorsResponse";
import {toast} from "react-toastify";

export class Stushbr {

    static showErrorPopup(errorMessage: ErrorsResponse) {
        const message = `Произошла одна или несколько ошибок: ${errorMessage.errors.map(value => {
            return `${(value.prop && `${value.prop}: ${value.msg}`) || value.msg}`;
        }).join('; ')}`
        toast.warn(message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
        });
    }
}