import * as React from 'react';
import { Route } from 'react-router';
import Layout from './containers/Layout';
import Home from './containers/Home';

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
    </Layout>
);
