import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { LocalizeProvider } from 'react-localize-redux';
import App from './components/App';
import configureStore from './helper/configure/configureStore';
import { configureLocalization } from './helper/configure/configureLocalization';
import axios from 'axios';

const store = configureLocalization(configureStore());

axios.interceptors.request.use((config) => {
    const state = store.getState();
    config.headers.Language = state.localize.options.defaultLanguage.substring(0, 2);

    return config;
});

ReactDOM.render(
    <Provider store={store}>
        <LocalizeProvider store={store}>
            <App />
        </LocalizeProvider>
    </Provider>,
    document.getElementById('root'),
);
