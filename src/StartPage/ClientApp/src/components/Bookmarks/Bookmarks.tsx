import React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';

import { ApplicationState } from '../../store';
import * as BookmarksStore from '../../store/Bookmarks';
import Bookmark from './Bookmark/Bookmark';
import BookmarkData from './BookmarkData/BookmarkData';
import Button from '../UI/Button/Button';

import classes from './Bookmarks.module.css';

type BookmarkProps =
    BookmarksStore.BookmarksState &
    typeof BookmarksStore.actionCreators &
    RouteComponentProps<{}>;

class Bookmarks extends React.PureComponent<BookmarkProps> {
    public componentDidMount() {
        this.ensureDataFetched();
    }

    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    private addBookmarkHandler = () => {
        this.props.showBookmark();
    }

    public render() {
        return (
            <div className={classes.Bookmarks}>
                <h1>Bookmarks</h1>
                <Button 
                    btnType=""
                    disabled={false}
                    clicked={this.addBookmarkHandler}>+</Button>
                <ul className={classes.BookmarkList}>
                    {this.props.bookmarks.map(props => <li key={props.id}><Bookmark {...props} /></li>)}
                </ul>
                {this.props.viewingBookmark ? <BookmarkData /> : null}
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
