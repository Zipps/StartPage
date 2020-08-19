import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface BookmarksState {
    isLoaded: boolean;
    bookmarks: Bookmark[];
    showCreateForm: boolean;
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

interface ShowBookmarkForm {
    type: 'SHOW_BOOKMARK_FORM'
}

interface CreateBookmarkAction {
    type: 'CREATE_BOOKMARK';
}

type KnownAction = RequestBookmarksAction | ReceiveBookmarksAction | ShowBookmarkForm | CreateBookmarkAction;

export const actionCreators = {
    requestBookmarks: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.bookmarks && !appState.bookmarks.isLoaded) {
            fetch(`bookmark`)
                .then(response => response.json() as Promise<Bookmark[]>)
                .then(data => dispatch({ type: 'RECEIVE_BOOKMARKS', bookmarks: data }))
        }
    },
    showBookmarkForm: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
       dispatch({ type: 'SHOW_BOOKMARK_FORM' });
    },
    createBookmark: (bookmark: Bookmark): AppThunkAction<KnownAction> => (dispatch, getState) => {
        var appState = getState();
        fetch(`bookmark`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(bookmark)
        })
            .then(response => response.json() as Promise<Bookmark>)
            .then(data => {
                let bookmarks: Bookmark[] = [];
                if (appState.bookmarks !== undefined) {
                    bookmarks = [...appState.bookmarks.bookmarks];
                }
                bookmarks.push(data);
                
                dispatch({ 
                    type: 'RECEIVE_BOOKMARKS', 
                    bookmarks: bookmarks}
            )});

    }
};

const unloadedState: BookmarksState = { bookmarks: [], isLoaded: false, showCreateForm: false };

export const reducer: Reducer<BookmarksState> = (state: BookmarksState | undefined, incomingAction: Action): BookmarksState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type){
        case 'REQUEST_BOOKMARKS':
            return {
                bookmarks: state.bookmarks,
                isLoaded: false,
                showCreateForm: state.showCreateForm 
            };
        case 'RECEIVE_BOOKMARKS':
            return {
                bookmarks: action.bookmarks,
                isLoaded: true,
                showCreateForm: state.showCreateForm
            };
        case 'SHOW_BOOKMARK_FORM':
            return {
                bookmarks: state.bookmarks,
                isLoaded: state.isLoaded,
                showCreateForm: true
            }
        case 'CREATE_BOOKMARK':
            return {
                bookmarks: state.bookmarks,
                isLoaded: false,
                showCreateForm: false
            }
    }

    return state;
}