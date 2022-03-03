import React from 'react';
import { GridCellProps } from '@progress/kendo-react-grid';
import moment from 'moment';

export const GridDateTimeCell = (e: GridCellProps) => {
    return <td>{moment(e.field && e.dataItem[e.field]).format('DD.MM.YYYY')}</td>;
};
