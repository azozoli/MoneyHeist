import { NotificationGroup, Notification } from '@progress/kendo-react-notification';
import { Fade } from '@progress/kendo-react-animation';
import React, { useEffect } from 'react';
//import { getTranslate, TranslateFunction } from 'react-localize-redux';
import { useSelector, useDispatch } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { toggleNotification as toggleMarketPartyNotification } from '../../stores/shared/actions';

interface IStateProps {
    //translate: TranslateFunction;
    marketPartyNotificationType: 'none' | 'success' | 'error' | 'warning' | 'info';
    marketPartyNotificationMessage: string;
}

const NotificationComponent: React.FC = () => {
    const dispatch = useDispatch();
    const { /* translate, */ marketPartyNotificationType, marketPartyNotificationMessage } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            //translate: getTranslate(state.localize),
            marketPartyNotificationType: state.shared.notificationType,
            marketPartyNotificationMessage: state.shared.notificationMessage,
        };
    });

    useEffect(() => {
        setTimeout(() => {
            dispatch(toggleMarketPartyNotification(false, 'none', ''));
        }, 5000);
    }, [dispatch]);

    return (
        <>
            <NotificationGroup
                className="notification-container"
                style={{
                    right: '50%',
                    top: 0,
                    alignItems: 'flex-start',
                    flexWrap: 'wrap-reverse',
                }}
            >
                <Fade enter={true} exit={true}>
                    {
                        <Notification
                            className="notification"
                            type={{ style: marketPartyNotificationType, icon: true }}
                            closable={true}
                            onClose={() => {
                                dispatch(toggleMarketPartyNotification(false, 'none', ''));
                            }}
                        >
                            <span>{marketPartyNotificationMessage}</span>
                        </Notification>
                    }
                </Fade>
            </NotificationGroup>
        </>
    );
};
export default NotificationComponent;
