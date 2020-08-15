import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as BookmarksStore from '../../store/Bookmarks';
import Bookmark from './Bookmark';

import styles from './Bookmarks.module.css';
import { RouteComponentProps } from 'react-router';

type BookmarkProps =
    BookmarksStore.BookmarksState &
    typeof BookmarksStore.actionCreators &
    RouteComponentProps<{}>;

class Bookmarks extends React.PureComponent<BookmarkProps> {
    public render() {
        return (
            <div className={styles.Bookmarks}>
                <h1>Bookmarks</h1>
                <ul>
                    {this.props.bookmarks.map(x => <li>{Bookmark(x)}</li>)}
                </ul>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.bookmarks,
    BookmarksStore.actionCreators
)(Bookmarks as any);
