import React, {Component} from "react";
import {ToastContainer} from "react-toastify";
import {Box, Container} from "react-bulma-components";

import "../custom.css"

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <Container p="1" style={{height: 1000}}>
                <Box display="flex" alignItems="center" flexDirection="column">
                    {this.props.children}
                </Box>
                <ToastContainer />
            </Container>
        );
    }
}
