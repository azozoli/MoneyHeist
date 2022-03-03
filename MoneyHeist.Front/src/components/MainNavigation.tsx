import React, { useState } from 'react';
import { Menu, MenuItem, MenuSelectEvent } from '@progress/kendo-react-layout';
import {
    getTranslate,
    getLanguages,
    TranslateFunction,
    Language,
    /* getActiveLanguage ,*/ setActiveLanguage,
    getActiveLanguage,
} from 'react-localize-redux';
import { useSelector, useDispatch } from 'react-redux';
import { AppState } from '../stores/rootReducer';
import { history } from '../helper/configure/history';
import { IVerifiedUser } from '../models/User';

interface IStateProps {
    translate: TranslateFunction;
    languages: Language[];
    verifiedUser: IVerifiedUser;
    currentLanguageCode: string;
}

const MainNavigation: React.FC = () => {
    const dispatch = useDispatch();
    const { translate, languages, currentLanguageCode, verifiedUser } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
            languages: getLanguages(state.localize),
            currentLanguageCode: getActiveLanguage(state.localize).code,
            verifiedUser: state.shared.verifiedUser,
        };
    });

    const menuTabs = {
        heist: 0,
        member: 1,
        skills: 2,
    };

    const defaultSelectedTabId = selectedPath(menuTabs.heist);

    function selectedPath(m: number) {
        switch (true) {
            case history.location.pathname === '/member':
                m = menuTabs.member;
                break;
            case history.location.pathname === '/skills':
                m = menuTabs.skills;
                break;
            case history.location.pathname === '/heist':
            default:
                m = menuTabs.heist;
                break;
        }
        return m;
    }

    const languageButtons: any[] = [];
    languages.forEach((lang) => {
        languageButtons.push(<MenuItem key={lang.code} data={{ language: lang.code }} text={lang.code} />);
    });

    function menuItemSelected(e: MenuSelectEvent) {
        if (e && e.item && e.item.data)
            if (e.item.data.language) {
                dispatch(setActiveLanguage(e.item.data.language));
            } else if (e.item.data.path) {
                const tabId = e.itemId.includes('_') ? e.itemId.substring(0, e.itemId.indexOf('_')) : e.itemId;
                setSelectedTabId(Number(tabId));
                if (e.item.data.name === 'logOut') {
                    window.localStorage.removeItem('jwt');
                    verifiedUser.token = undefined;
                }
                history.push(e.item.data.path);
            }
    }
    const [selectedTabId, setSelectedTabId] = useState(defaultSelectedTabId);

    const menuItemClass = 'custom-menu-item';
    const menuItemActiveClass = 'custom-menu-item active';

    return (
        <div>
            <Menu onSelect={menuItemSelected} className="main-menu">
                <MenuItem
                    text={translate('heist').toString()}
                    data={{ path: '/partner' }}
                    cssClass={selectedTabId === menuTabs.heist ? menuItemActiveClass : menuItemClass}
                />
                <MenuItem
                    text={translate('member').toString()}
                    data={{ path: '/member' }}
                    cssClass={selectedTabId === menuTabs.member ? menuItemActiveClass : menuItemClass}
                />
                <MenuItem text={translate('skills').toString()} cssClass={selectedTabId === menuTabs.skills ? menuItemActiveClass : menuItemClass} />

                {/* <MenuItem text={'Log out'} data={{ path: '/', name: 'logOut' }} /> */}

                <MenuItem text={currentLanguageCode}>{languageButtons}</MenuItem>
            </Menu>
        </div>
    );
};

export default MainNavigation;
