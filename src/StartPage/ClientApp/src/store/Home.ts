import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface HomeState {
    showBookmarks: boolean;
}

export const actionCreators = {

}

const unloadedState: HomeState = {showBookmarks: true};

export const reducer: Reducer<HomeState> = (state: HomeState | undefined): HomeState => {
    if (state === undefined) {
        return unloadedState;
    }

    return state;
}