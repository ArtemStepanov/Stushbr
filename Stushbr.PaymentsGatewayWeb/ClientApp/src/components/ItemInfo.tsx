import {Button, Card} from "react-bulma-components";
import {Item} from "../models/Item";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faLeaf} from "@fortawesome/free-solid-svg-icons";
import {useState} from "react";

function ItemInfo(props: { item: Item, buyButtonClickHandler: () => void }) {
    const [loading, setLoading] = useState<boolean>(false)

    const handleBuyButtonClick = async () => {
        setLoading(true)
        props.buyButtonClickHandler()
        setLoading(false)
    }

    return (
        <Card style={{width: 300}}>
            <Card.Header>
                <Card.Header.Title>{props.item.displayName}</Card.Header.Title>
                <Card.Header.Icon>
                    <FontAwesomeIcon icon={faLeaf} />
                </Card.Header.Icon>
            </Card.Header>
            <Card.Image src={props.item.imageUrl} />
            <Card.Content>
                {props.item.description}
            </Card.Content>
            <Card.Footer>
                <Card.Footer.Item>
                    <Button onClick={() => handleBuyButtonClick()} loading={loading}>Купить
                        - {props.item.price}₽</Button>
                </Card.Footer.Item>
            </Card.Footer>
        </Card>
    );
}

export default ItemInfo;