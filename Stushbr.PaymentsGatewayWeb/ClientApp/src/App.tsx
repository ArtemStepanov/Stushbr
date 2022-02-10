import React, {Component} from 'react';
import {Route} from 'react-router-dom';
import {Layout} from './components/Layout';

import './custom.css'
import 'react-toastify/dist/ReactToastify.css';
import 'bulma/css/bulma.min.css';

import ItemsPage from "./components/ItemsPage";
import ItemPage from "./components/ItemPage";

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={ItemsPage} />
                <Route exact path='/:id' component={ItemPage} />
            </Layout>
        );
    }
}
