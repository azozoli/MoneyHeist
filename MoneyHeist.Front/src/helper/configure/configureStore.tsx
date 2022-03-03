import { createStore, applyMiddleware } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import thunkMiddleware from 'redux-thunk';
import { createLogger } from 'redux-logger';
import { rootReducer } from '../../stores/rootReducer';
import frontendSettings from '../frontendSettings';

export default function configureStore() {
    if (frontendSettings.IsProduction) return createStore(rootReducer, applyMiddleware(thunkMiddleware));
    else return createStore(rootReducer, composeWithDevTools(applyMiddleware(thunkMiddleware, createLogger())));
}
