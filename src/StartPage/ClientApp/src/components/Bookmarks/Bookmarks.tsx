import React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as BookmarksStore from '../../store/Bookmarks';
import Bookmark from './Bookmark/Bookmark';
import BookmarkForm from './BookmarkForm/BookmarkForm';
import { RouteComponentProps } from 'react-router';

import classes from './Bookmarks.module.css';

type BookmarkProps =
    BookmarksStore.BookmarksState &
    typeof BookmarksStore.actionCreators &
    RouteComponentProps<{}>;

class Bookmarks extends React.PureComponent<BookmarkProps> {
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    public render() {
        return (
            <div className={classes.Bookmarks}>
                <h1>Bookmarks</h1>
                <button onClick={this.props.showBookmarkForm}>Add</button>
                <ul>
                    {this.props.bookmarks.map(props => <li key={props.id}>(<Bookmark {...props} />)</li>)}
                </ul>
                {this.props.showCreateForm ? (<BookmarkForm />) : null}
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
