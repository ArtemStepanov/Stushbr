import React, {Component} from 'react';
import './NavMenu.css';
import {Container, Navbar} from "react-bulma-components";

interface T {
    collapsed: boolean
}

export class NavMenu extends Component<any, T> {
    static displayName = NavMenu.name;

    constructor(props: any) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <header>
                <Navbar>
                    <Container>
                        <Navbar.Brand>Stushbr.PaymentsGateway</Navbar.Brand>
                    </Container>
                </Navbar>
            </header>
        );
    }
}
