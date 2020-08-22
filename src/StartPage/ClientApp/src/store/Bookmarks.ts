import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface BookmarksState {
    isLoaded: boolean;
    bookmarks: Bookmark[];
    loadedBookmark?: Bookmark;
    editMode: boolean;
}

export interface Bookmark {
    id?: string;
    title?: string;
    imageUrl?: string;
    url?: string;
}

interface RequestBookmarksAction {
    type: 'REQUEST_BOOKMARKS';
}

interface ReceiveBookmarksAction {
    type: 'RECEIVE_BOOKMARKS';
    bookmarks: Bookmark[];
}

interface SaveBookmarkAction {
    type: 'SAVE_BOOKMARK';
}

interface DeleteBookmarkAction {
    type: 'DELETE_BOOKMARK';
    id: string;
}

interface ShowBookmarkAction {
    type: 'SHOW_BOOKMARK';
    bookmark?: Bookmark;
}

interface EditModeAction {
    type: 'EDIT_BOOKMARKS';
    edit: boolean;
}

type KnownAction = RequestBookmarksAction | ReceiveBookmarksAction | SaveBookmarkAction | 
                   ShowBookmarkAction | DeleteBookmarkAction | EditModeAction;

export const actionCreators = {
    requestBookmarks: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.bookmarks && !appState.bookmarks.isLoaded) {
            fetch(`bookmark`)
                .then(response => response.json() as Promise<Bookmark[]>)
                .then(data => dispatch({ type: 'RECEIVE_BOOKMARKS', bookmarks: data }))
        }
    },
    saveBookmark: (bookmark: Bookmark): AppThunkAction<KnownAction> => (dispatch, getState) => {
        let endpoint = 'bookmark';
        if (bookmark.id) {
            endpoint += `/${bookmark.id}`;
        }
        fetch(endpoint, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(bookmark)
        })
            .then(() => {
                dispatch({type: 'SHOW_BOOKMARK', bookmark: bookmark});
                dispatch({type: 'REQUEST_BOOKMARKS'});
            });

    },
    deleteBookmark: (id: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        fetch(`bookmark/${id}`, {
            method: 'DELETE'
        })
            .then(() => {
                dispatch({type: 'DELETE_BOOKMARK', id: id});
            });
    },
    showBookmark: (bookmark?: Bookmark, hide: boolean = false): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({type: 'SHOW_BOOKMARK', bookmark: bookmark});
    },
    showEditMode: (editMode: boolean = true): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({type: 'EDIT_BOOKMARKS', edit: editMode});
    }
};

const unloadedState: BookmarksState = { bookmarks: [], isLoaded: false, loadedBookmark: undefined, editMode: false};

export const reducer: Reducer<BookmarksState> = (state: BookmarksState = unloadedState, 
                                                 incomingAction: Action): BookmarksState => {
    const action = incomingAction as KnownAction;
    switch (action.type){
        case 'REQUEST_BOOKMARKS':
            return {
                ...state,
                isLoaded: false
            };
        case 'RECEIVE_BOOKMARKS':
            return {
                ...state,
                bookmarks: action.bookmarks,
                isLoaded: true
            };
        case 'SAVE_BOOKMARK':
            return {
                ...state,
                isLoaded: false,
                loadedBookmark: undefined
            };
        case 'DELETE_BOOKMARK':
            const bookmarks: Bookmark[] = [];
            state.bookmarks.map(x => {
                if (x.id !== action.id) {
                    bookmarks.push(x);
                }
            });
            
            return {
                ...state,
                bookmarks: bookmarks,
                isLoaded: false,
                loadedBookmark: undefined
            }
        case 'SHOW_BOOKMARK':
            return {
                ...state,
                loadedBookmark: action.bookmark
            };
        case 'EDIT_BOOKMARKS':
            return {
                ...state,
                editMode: action.edit
            };
    }

    return state;
}