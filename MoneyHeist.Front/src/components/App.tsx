import React from 'react';
import { Route, Switch, Router } from 'react-router-dom';
import { useSelector, useDispatch } from 'react-redux';
import { getActiveLanguage } from 'react-localize-redux';

import MainNavigation from './MainNavigation';
import { AppState } from '../stores/rootReducer';
import { history } from '../helper/configure/history';

//Components import
import NotificationComponent from './shared/NotificationComponent';

// styles import
import './../styles/kendo-adnet-theme/all.css';
import './../styles/kendo-adnet-theme/variables.scss';
import './../styles/scss/styles.scss';

// kendo globaliztion
import { IntlProvider, load, loadMessages, LocalizationProvider } from '@progress/kendo-react-intl';
import likelySubtags from 'cldr-core/supplemental/likelySubtags.json';
import currencyData from 'cldr-core/supplemental/currencyData.json';
import weekData from 'cldr-core/supplemental/weekData.json';
// croatian
import hrNumbers from 'cldr-numbers-full/main/hr/numbers.json';
import hrLocalCurrency from 'cldr-numbers-full/main/hr/currencies.json';
import hrCaGregorian from 'cldr-dates-full/main/hr/ca-gregorian.json';
import hrDateFields from 'cldr-dates-full/main/hr/dateFields.json';
import kendoMessagesHR from './../localization/kendoMessagesLocalization/kendoMessages_hr-HR.json';
// english
import enNumbers from 'cldr-numbers-full/main/en-GB/numbers.json';
import enLocalCurrency from 'cldr-numbers-full/main/en-GB/currencies.json';
import enCaGregorian from 'cldr-dates-full/main/en-GB/ca-gregorian.json';
import enDateFields from 'cldr-dates-full/main/en-GB/dateFields.json';
import kendoMessagesEN from './../localization/kendoMessagesLocalization/kendoMessages_en-GB.json';
import DialogComponent from './shared/DialogComponent';
import { toggleDeleteDialog } from '../stores/shared/actions';
import HeistComponent from './heists/HeistComponent';

load(
    likelySubtags,
    currencyData,
    weekData,
    hrNumbers,
    hrLocalCurrency,
    hrCaGregorian,
    hrDateFields,
    enNumbers,
    enLocalCurrency,
    enCaGregorian,
    enDateFields,
);
loadMessages(kendoMessagesHR, 'HR');
loadMessages(kendoMessagesEN, 'GB');

interface IStateProps {
    currentLanguageCode: string;
    notificationIsOpen: boolean;
    deleteDialogIsOpen: boolean;
    deleteDialogAction: any;
}

const App: React.FC = () => {
    const dispatch = useDispatch();

    const { currentLanguageCode, deleteDialogIsOpen, deleteDialogAction, notificationIsOpen } = useSelector<AppState, IStateProps>(
        (state: AppState) => {
            return {
                currentLanguageCode: getActiveLanguage(state.localize).code,
                deleteDialogIsOpen: state.shared.deleteDialogIsOpen,
                deleteDialogAction: state.shared.deleteDialogAction,
                notificationIsOpen: state.shared.notificationIsOpen,
            };
        },
    );

    //USER LOGIN-------------------------------------------------------
    /* const [tokenAvaliable, setTokenAvaliable] = useState(false);

	useEffect(() => {
		if (window.localStorage.getItem('jwt')) {
			setTokenAvaliable(true);
		} else {
			setTokenAvaliable(false);
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [window.localStorage.getItem('jwt')]); */
    //------------------------------------------------------------------

    return (
        <LocalizationProvider language={currentLanguageCode}>
            <IntlProvider locale={currentLanguageCode}>
                <>
                    <div id="overlay" />
                    {deleteDialogIsOpen && (
                        <>
                            {/* <div id="overlay" style={{ display: 'block' }} /> */}
                            <DialogComponent
                                dialogAction={(id: number) => {
                                    dispatch(deleteDialogAction(id));
                                }}
                                dialogCloseAction={(boolean: boolean, id: number) => {
                                    dispatch(toggleDeleteDialog(boolean, id));
                                }}
                            />
                        </>
                    )}

                    {notificationIsOpen && <NotificationComponent />}
                    <MainNavigation />
                    <Router history={history}>
                        <Switch>
                            {/* USER LOGIN */}
                            {/* {!tokenAvaliable && <Route path="/" component={LoginForm} />} */}
                            {true && (
                                <>
                                    <Route exact path="/" component={HeistComponent} />
                                    <Route exact path="/heist" component={HeistComponent} />
                                    <Route exact path="/member" />
                                    <Route path="/skills" />
                                </>
                            )}
                        </Switch>
                    </Router>
                </>
            </IntlProvider>
        </LocalizationProvider>
    );
};

export default App;
