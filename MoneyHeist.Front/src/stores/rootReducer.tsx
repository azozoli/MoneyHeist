import { combineReducers } from 'redux';
import { localizeReducer } from 'react-localize-redux';
import { heistReducer } from './heist/reducer';

export const rootReducer = combineReducers({
    localize: localizeReducer,
    heists: heistReducer,
});

export type AppState = ReturnType<typeof rootReducer>;
