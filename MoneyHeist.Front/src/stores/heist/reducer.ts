import { IHeistState } from './interfaces';
import { IHeistActionType } from './interfaces';
import * as actionTypes from './actionTypes';

const initialState: IHeistState = {
    heists: [],
    heistEditFormWindowIsOpen: false,
    heist: null,
    selectedHeist: null,
};

export function heistReducer(state: IHeistState = initialState, action: IHeistActionType): IHeistState {
    switch (action.type) {
        case actionTypes.GET_HEISTS:
            return {
                ...state,
                heists: action.heists,
            };
        case actionTypes.SHOW_EDIT_HEIST_FORM_WINDOW:
            return {
                ...state,
                heistEditFormWindowIsOpen: action.heistEditFormWindowIsOpen,
            };
        case actionTypes.UPDATE_HEIST:
            return {
                ...state,
                heist: action.heist,
            };
        case actionTypes.SET_SELECTED_HEIST:
            return {
                ...state,
                selectedHeist: action.selectedHeist,
            };
        default:
            return state;
    }
}
