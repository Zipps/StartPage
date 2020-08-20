import React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';

import { ApplicationState } from '../../store';
import * as BookmarksStore from '../../store/Bookmarks';
import Bookmark from './Bookmark/Bookmark';
import BookmarkData from './BookmarkData/BookmarkData';

import classes from './Bookmarks.module.css';

type BookmarkProps =
    BookmarksStore.BookmarksState &
    typeof BookmarksStore.actionCreators &
    RouteComponentProps<{}>;

class Bookmarks extends React.PureComponent<BookmarkProps> {
    state = {
        viewingBookmark: false
    }

    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    private addBookmarkHandler = () => {
        this.setState( { viewingBookmark: true} );
    }

    public render() {
        return (
            <div className={classes.Bookmarks}>
                <h1>Bookmarks</h1>
                <button onClick={this.addBookmarkHandler}>Add</button>
                <ul className={classes.BookmarkList}>
                    {this.props.bookmarks.map(props => <li key={props.id}>(<Bookmark {...props} />)</li>)}
                </ul>
                {this.state.viewingBookmark ? (<BookmarkData />) : null}
            </div>
        );
    }

    private ensureDataFetched() {
        this.props.requestBookmarks();
    }
}

export default connect(
    (state: ApplicationState) => state.bookmarks,
    BookmarksStore.actionCreators
)(Bookmarks as any);
