import React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';

import { ApplicationState } from '../../store';
import * as BookmarksStore from '../../store/Bookmarks';
import Bookmark from './Bookmark/Bookmark';
import BookmarkData from './BookmarkData/BookmarkData';
import Button from '../UI/Button/Button';
import SVGIcon from '../UI/SVGIcon/SVGIcon';

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
        this.props.showBookmark({});
    }

    private editBookmarkHandler = (id?: string) => {
        if (!id) return;

        const bookmark = this.props.bookmarks.find(x => x.id === id);
        this.props.showBookmark(bookmark || {});
    }

    private editModeHandler = (show: boolean) => {
        this.props.showEditMode(show);
    }

    public render() {
        return (
            <div className={classes.Bookmarks}>
                <div className={classes.ActionBar}>
                <div className={classes.Actions}>
                    {!this.props.editMode 
                    ?
                    <React.Fragment>
                    <Button btnType="Icon" clicked={this.addBookmarkHandler}>
                        <SVGIcon 
                            name='plus'
                            width='18px'
                            height='18px'
                            fill='#eee'
                            />
                        </Button>
                    <Button btnType="Icon" clicked={() => this.editModeHandler(true)}>
                        <SVGIcon
                            name='gear'
                            width='18px'
                            height='18px'
                            fill='#eee'
                            />                            
                    </Button>
                    </React.Fragment>
                    : <Button btnType="Icon" clicked={() => this.editModeHandler(false)}>
                        <SVGIcon
                            name='tick'
                            width='18px'
                            height='18px'
                            fill='#1F85DE'
                        />
                      </Button>}
                </div>
                </div>
                <ul className={classes.BookmarkList}>
                    {this.props.bookmarks.map(props => 
                        <li key={props.id} className={classes.ListElement}>
                            <Bookmark {...props} />
                            {this.props.editMode 
                                ? <div className={classes.Overlay}
                                       onClick={() => this.editBookmarkHandler(props.id)}>
                                    <SVGIcon
                                        name='gear'
                                        width='32px'
                                        height='32px'
                                        fill='#eee'/>
                                </div> 
                                : null}
                        </li>
                    )}
                </ul>
                {this.props.loadedBookmark ? <BookmarkData /> : null}
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
