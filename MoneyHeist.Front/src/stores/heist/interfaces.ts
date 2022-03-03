import { IHeist } from '../../models/Heist';
import * as actionTypes from './actionTypes';

export interface IHeistState {
    heists: IHeist[];
    heistEditFormWindowIsOpen: boolean;
    heist: IHeist | null;
    selectedHeist: IHeist | null;
}

interface IShowEditHeistFormWindow {
    type: typeof actionTypes.SHOW_EDIT_HEIST_FORM_WINDOW;
    heistEditFormWindowIsOpen: boolean;
}

interface IGetHeistsSuccess {
    type: typeof actionTypes.GET_HEISTS;
    heists: IHeist[];
}

interface IUpdateHeistSuccess {
    type: typeof actionTypes.UPDATE_HEIST;
    heist: IHeist;
}

interface ISetSelectedHeist {
    type: typeof actionTypes.SET_SELECTED_HEIST;
    selectedHeist: IHeist;
}

export type IHeistActionType = IGetHeistsSuccess | IShowEditHeistFormWindow | IUpdateHeistSuccess | ISetSelectedHeist;
