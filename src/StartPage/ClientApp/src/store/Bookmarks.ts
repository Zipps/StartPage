import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface BookmarksState {
    isLoaded: boolean;
    bookmarks: Bookmark[];
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

type KnownAction = RequestBookmarksAction | ReceiveBookmarksAction | SaveBookmarkAction;

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

const unloadedState: BookmarksState = { bookmarks: [], isLoaded: false };

export const reducer: Reducer<BookmarksState> = (state: BookmarksState | undefined, incomingAction: Action): BookmarksState => {
    if (state === undefined) {
        return unloadedState;
    }

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
                isLoaded: false
            }
    }

    return state;
}