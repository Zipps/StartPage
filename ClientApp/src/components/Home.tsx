import React, { Component } from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as HomeStore from '../store/Home';
import Bookmarks from './Bookmarks/Bookmarks';

type HomeProps =
    HomeStore.HomeState &
    typeof HomeStore.actionCreators &
    RouteComponentProps<{}>;

class Home extends Component<HomeProps> {
    public render() {
        return (
            this.props.showBookmarks ? <Bookmarks/> : null
        );
    }
}
export default connect(
    (state: ApplicationState) => state.home,
    HomeStore.actionCreators
)(Home as any);
