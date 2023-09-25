import {ItemType} from "./ItemType";

export interface Item {
    id: string;
    displayName: string;
    description: string;
    imageUrl: string;
    price: number;
    type: ItemType;
    isEnabled: boolean;
    availableSince: Date;
    availableBefore: Date | null;
}