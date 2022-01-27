import React from 'react';
import {Item} from "../models/Item";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faAngleDown} from "@fortawesome/free-solid-svg-icons";
import {Dropdown, Icon} from "react-bulma-components";

function ItemsDropdown(props: { items: Item[], onDropdownItemChange: (item: Item) => void }) {
    return (
        <Dropdown
            label="За вдохновением"
            onChange={props.onDropdownItemChange!}
            icon={<Icon><FontAwesomeIcon icon={faAngleDown} /></Icon>}
        >
            {props.items.map(item => <Dropdown.Item key={item.id} value={item}>{item.displayName}</Dropdown.Item>)}
        </Dropdown>
    );
}

export default ItemsDropdown;
