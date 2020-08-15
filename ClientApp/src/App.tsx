import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';

import styles from './App.module.css'

export default () => (
    <div className={styles.App}>
        <Layout>
            <Route exact path='/' component={Home} />
        </Layout>
    </div>
);
