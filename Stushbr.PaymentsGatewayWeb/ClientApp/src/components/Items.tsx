import React, {useEffect, useState} from 'react';
import {Api} from "../common/Api";
import {Item} from "../models/Item";

function Items(props: any) {
    const fetchItems = async () => {
        setLoading(true)
        const items = await Api.getAvailableItems()
        setLoading(false);
        setItems(items)
    }

    const [loading, setLoading] = useState<boolean>(false)
    const [items, setItems] = useState<Item[] | null>()

    useEffect(() => {
        document.title = "Wellcum";

        void fetchItems()
    }, []);

    return (
        <div>
        </div>
    );
}

export default Items;