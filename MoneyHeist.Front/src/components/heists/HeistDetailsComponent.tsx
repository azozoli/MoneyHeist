import React, { useEffect, useState } from 'react';
import { Grid, GridCellProps, GridColumn } from '@progress/kendo-react-grid';
import { TranslateFunction, getTranslate } from 'react-localize-redux';
import { useDispatch, useSelector } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { IHeist } from '../../models/Heist';
import { showEditHeistFormWindow } from '../../stores/heist/actions';
import { Button } from '@progress/kendo-react-buttons';
import EditHeistForm from './EditHeistForm';

interface IStateProps {
    translate: TranslateFunction;
    heistEditFormWindowIsOpen: boolean;
    heistDetails: IHeist | null;
}

const HeistDetailsComponent: React.FC = () => {
    const dispatch = useDispatch();
    const { translate, heistEditFormWindowIsOpen, heistDetails } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
            heistEditFormWindowIsOpen: state.heists.heistEditFormWindowIsOpen,
            heistDetails: state.heists.selectedHeist,
        };
    });

    const HeistGridActions = (props: GridCellProps) => {
        return (
            <td>
                <Button onClick={() => dispatch(showEditHeistFormWindow(true))} disabled={!props.dataItem}>
                    {translate('edit')}
                </Button>
                {/* <Button disabled={!props.dataItem}>{translate('changeOIB')}</Button> */}
            </td>
        );
    };

    const [heistDetailsState, setHeistDetailsState] = useState(heistDetails);

    useEffect(() => {
        setHeistDetailsState(heistDetails);
    }, [heistDetails]);

    return (
        <div>
            {heistEditFormWindowIsOpen && heistDetailsState && <EditHeistForm heistDetails={heistDetailsState} />}
            <div className="col-12">
                <h3>{heistDetailsState ? heistDetailsState.name : translate('Heist')}</h3>
                <Grid data={[heistDetailsState]}>
                    <GridColumn field="id" title={translate('id').toString()} />
                    <GridColumn field="name" title={translate('name').toString()} />
                    <GridColumn field="location" title={translate('type').toString()} />
                    <GridColumn field="startTime" title={translate('startTime').toString()} />
                    <GridColumn field="endTime" title={translate('endTime').toString()} />
                    <GridColumn field="" title={translate('actions').toString()} cell={HeistGridActions} />
                </Grid>
            </div>
        </div>
    );
};
export default HeistDetailsComponent;
