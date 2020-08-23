import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

export interface UserState {
    isLoaded: boolean;
    user: User
}

export interface User {
    userId?: string;
    username: string;
    emailAddress: string;
    password: string;
}

interface RequestUserAction {
    type: 'REQUEST_USER';
}

interface ReceiveUserAction {
    type: 'RECEIVE_USER',
    user: User
}

type KnownAction = RequestUserAction | ReceiveUserAction;

export const actionCreators = {
    requestUser: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    }
}

const unloadedState: UserState = { isLoaded: false };

export const reducer: Reducer<UserState> = (state: UserState = unloadedState,
                                            incomingAction: Action): UserState => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_USER':
            return {
                ...state,
                isLoaded: false
            };
        case 'RECEIVE_USER':
            return {
                ...state,
                isLoaded: true
            };
    }
    return state;
}