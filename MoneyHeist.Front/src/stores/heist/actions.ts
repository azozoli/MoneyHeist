import { requests } from '../agent';
import * as actionTypes from './actionTypes';
import { ThunkAction } from 'redux-thunk';
import { AppState } from '../rootReducer';
import { Action } from 'redux';
import { IHeist } from '../../models/Heist';
import { toggleNotification, turnOffMainLoading, turnOnMainLoading } from '../shared/actions';
import { IHeistActionType } from './interfaces';

const apiActions = {
    getHeists: (): Promise<IHeist[]> => requests.get(`Heist/GetHeists`),
    updateHeist: (Heist: IHeist): Promise<IHeist> => requests.put('Heist/UpdateHeist', Heist),
};

export const showEditHeistFormWindow = (isOpen: boolean): IHeistActionType => {
    const displayStyle = isOpen ? 'block' : 'none';
    document.getElementById('overlay')!.style.display = displayStyle;
    return {
        type: actionTypes.SHOW_EDIT_HEIST_FORM_WINDOW,
        heistEditFormWindowIsOpen: isOpen,
    };
};

export const getHeists = (): ThunkAction<void, AppState, unknown, Action<string>> => async (dispatch) => {
    try {
        dispatch(turnOnMainLoading());
        dispatch(success(await apiActions.getHeists()));
    } catch (error) {
        dispatch(turnOffMainLoading());
        console.log(error);
    }

    function success(heists: IHeist[]): IHeistActionType {
        return {
            type: actionTypes.GET_HEISTS,
            heists: heists,
        };
    }
};
export const updateHeist =
    (heist: any): ThunkAction<void, AppState, unknown, Action<string>> =>
    async (dispatch) => {
        try {
            dispatch(turnOnMainLoading());
            dispatch(updateHeistSuccess(await apiActions.updateHeist(heist)));
        } catch (error) {
            dispatch(turnOffMainLoading());
            dispatch(toggleNotification(true, 'error', `Heist ${heist.name} not updated`));
            console.log(error);
        }

        function updateHeistSuccess(Heist: IHeist): IHeistActionType {
            dispatch(showEditHeistFormWindow(false));
            dispatch(toggleNotification(true, 'success', `Heist ${Heist.name} succesfully updated`));
            return {
                type: actionTypes.UPDATE_HEIST,
                heist: heist,
            };
        }
    };

export const setSelectedHeist = (heist: IHeist): IHeistActionType => {
    return {
        type: actionTypes.SET_SELECTED_HEIST,
        selectedHeist: heist,
    };
};
