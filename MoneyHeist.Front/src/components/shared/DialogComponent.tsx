import React from 'react';
import { useSelector } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { Dialog, DialogActionsBar } from '@progress/kendo-react-dialogs';

interface IProps {
    dialogAction: (id: any) => unknown;
    dialogActionDocuments?: () => void;
    dialogCloseAction: (boolean: boolean, id: 0) => unknown;
}

interface IStateProps {
    selectedId: number;
    dialogAction?: (id: number) => unknown;
    dialogMessage: string;
}

const DialogComponent: React.FC<IProps> = ({ dialogAction, dialogCloseAction, dialogActionDocuments }: IProps) => {
    const { selectedId, dialogMessage } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            selectedId: state.shared.selectedId,
            dialogMessage: state.shared.dialogMessage,
        };
    });

    return (
        <>
            <Dialog className="dialog" title={'Please confirm'} onClose={() => dialogCloseAction(false, 0)}>
                <p style={{ margin: '25px', textAlign: 'center' }}>{dialogMessage !== '' ? dialogMessage : 'Are you sure you want to delete?'}</p>
                <DialogActionsBar>
                    <button className="k-button" onClick={() => dialogCloseAction(false, 0)}>
                        No
                    </button>
                    <button
                        className="k-button"
                        onClick={() => (dialogActionDocuments !== undefined ? dialogActionDocuments() : dialogAction(selectedId))}
                    >
                        Yes
                    </button>
                </DialogActionsBar>
            </Dialog>
        </>
    );
};

export default DialogComponent;
