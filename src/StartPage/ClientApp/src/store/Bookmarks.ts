import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface BookmarksState {
    isLoading: boolean;
    bookmarks: Bookmark[];
}

export interface Bookmark {
    id: string;
    title?: string;
    imageUrl?: string;
    url: string;
}

interface RequestBookmarksAction {
    type: 'REQUEST_BOOKMARKS';
}

interface ReceiveBookmarksAction {
    type: 'RECEIVE_BOOKMARKS';
    bookmarks: Bookmark[];
}

type KnownAction = RequestBookmarksAction | ReceiveBookmarksAction;

export const actionCreators = {
    requestBookmarks: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.bookmarks && appState.bookmarks.bookmarks.length === 0) {
            fetch(`bookmark`)
                .then(response => response.json() as Promise<Bookmark[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_BOOKMARKS', bookmarks: data });
                })
        }
    }
};

const unloadedState: BookmarksState = { bookmarks: [], isLoading: false };

export const reducer: Reducer<BookmarksState> = (state: BookmarksState | undefined, incomingAction: Action): BookmarksState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type){
        case 'REQUEST_BOOKMARKS':
            return {
                bookmarks: state.bookmarks,
                isLoading: true
            };
        case 'RECEIVE_BOOKMARKS':
            return {
                bookmarks: action.bookmarks,
                isLoading: false
            };
    }

    return state;
}