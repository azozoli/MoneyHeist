import { initialize } from 'react-localize-redux';
import { renderToStaticMarkup } from 'react-dom/server';

const languages = require('../../localization/languages.json');
const translations = require('./../../localization/translations.json');

const languagesList: any = [];
let defaultLanguageCode = '';
Object.keys(languages).forEach((language) => {
    const lang = languages[language];
    if (lang && lang.isDefault === 'true') {
        defaultLanguageCode = lang.code;
    }
    languagesList.push(languages[language]);
});

export function configureLocalization(store: any) {
    store.dispatch(
        initialize({
            languages: languagesList,
            translation: translations,
            options: { renderToStaticMarkup, defaultLanguage: defaultLanguageCode },
        }),
    );

    return store;
}
