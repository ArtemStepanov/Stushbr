import React, {Component} from 'react';
import {ToastContainer} from "react-toastify";
import {Container} from "react-bulma-components";
import {NavMenu} from "./NavMenu";

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <Container>
                <NavMenu />
                {this.props.children}
                <ToastContainer />
            </Container>
        );
    }
}
