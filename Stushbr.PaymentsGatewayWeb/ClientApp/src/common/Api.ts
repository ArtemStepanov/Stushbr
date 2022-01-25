import {Item} from "../models/Item";
import {Stushbr} from "./Stushbr";
import {ErrorsResponse} from "../models/ErrorsResponse";

export class Api {

    public static async getAvailableItems(): Promise<Item[] | null> {
        const data = await this.fetchWithHandling<Item[]>('items/available')
        return data;
    }

    private static async fetchWithHandling<T>(url: string, options?: RequestInit | undefined): Promise<T | null> {
        const response = await fetch(url, options || {})
        if (response.status >= 200 && response.status <= 299) {
            return await response.json();
        } else {
            const error: ErrorsResponse = await response.json();
            Stushbr.showErrorPopup(error)
            return null;
        }

        return null;
    }
}