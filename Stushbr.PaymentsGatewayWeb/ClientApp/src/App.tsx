import React, {Component} from 'react';
import {Route} from 'react-router-dom';
import {Layout} from './components/Layout';

import './custom.css'
import 'react-toastify/dist/ReactToastify.css';
import 'bulma/css/bulma.min.css';

import ItemsPage from "./components/ItemsPage";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={ItemsPage} />
            </Layout>
        );
    }
}
