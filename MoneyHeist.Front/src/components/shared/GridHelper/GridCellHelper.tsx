import React from 'react';
import { GridCellProps } from '@progress/kendo-react-grid';
import { dateFormat, dateTimeFormat, getDateString } from '../../../helper/helperFunctions';
import { Calendar, CalendarProps } from '@progress/kendo-react-dateinputs';
import { Checkbox } from '@progress/kendo-react-inputs';

export const dateCell = (e: GridCellProps) => {
    return <td>{getDateString(e.field && e.dataItem[e.field], dateFormat)}</td>;
};

export const dateTimeCell = (e: GridCellProps) => {
    return <td>{getDateString(e.field && e.dataItem[e.field], dateTimeFormat)}</td>;
};

export const customCalendar = (e: CalendarProps) => {
    return <Calendar bottomView="year" topView="year" value={e.value} onChange={e.onChange} onBlur={e.onBlur} />;
};

export const checkBoxCell = (e: GridCellProps) => {
    return (
        <td style={{ textAlign: 'center' }}>
            <Checkbox checked={e.field && e.dataItem[e.field]} disabled label="" />
        </td>
    );
};
