import React from 'react';
import { Button } from '@progress/kendo-react-buttons';

interface IProps {
    editAction: () => void;
    deleteAction: () => void;
}

const GridEditDeleteButtons: React.FC<IProps> = ({ editAction, deleteAction }: IProps) => {
    return (
        <td style={{ textAlign: 'center' }}>
            <Button
                className=" k-button gridButton"
                onClick={() => {
                    editAction();
                }}
            >
                <span className="k-icon k-i-edit" />
            </Button>

            <Button
                className="k-button gridButton gridDelete"
                style={{ marginLeft: '10px' }}
                onClick={() => {
                    deleteAction();
                }}
            >
                <span className="k-icon k-i-delete" />
            </Button>
        </td>
    );
};
export default GridEditDeleteButtons;
