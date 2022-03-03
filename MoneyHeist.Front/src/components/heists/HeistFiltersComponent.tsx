import React from 'react';
import { getTranslate, TranslateFunction } from 'react-localize-redux';
import { useSelector } from 'react-redux';
import { AppState } from '../../stores/rootReducer';
import { ComboBoxChangeEvent, MultiColumnComboBox, ComboBoxFilterChangeEvent } from '@progress/kendo-react-dropdowns';
import { IHeist } from '../../models/Heist';
import { filterBy, FilterDescriptor } from '@progress/kendo-data-query';

interface IHeistFiltersProps {
    heistFilterChange: any;
    heistFilters: any;
}

interface IStateProps {
    translate: TranslateFunction;
    heists: IHeist[] | undefined;
}

const HeistFiltersComponent: React.FC<IHeistFiltersProps> = ({ heistFilterChange, heistFilters }: IHeistFiltersProps) => {
    // const dispatch = useDispatch();
    const { translate, heists } = useSelector<AppState, IStateProps>((state: AppState) => {
        return {
            translate: getTranslate(state.localize),
            heists: state.heists.heists,
        };
    });

    const [filter, setFilter] = React.useState<FilterDescriptor>();

    const handleFilterChange = (event: ComboBoxFilterChangeEvent) => {
        if (event) {
            setFilter(event.filter);
        }
    };

    return (
        <>
            <h4 style={{ marginLeft: '5px' }}>{translate('filters')}</h4>
            <div className="col-12 filterContainer">
                <div className="col-3">
                    {' '}
                    <div>
                        <span className="col-5">
                            <label>{translate('insertHeistName') + ': '}</label>
                        </span>
                        <MultiColumnComboBox
                            className="col-6"
                            data={filter ? filterBy(heists ? heists : [], filter) : heists}
                            textField="name"
                            filterable={true}
                            onFilterChange={handleFilterChange}
                            onChange={(event: ComboBoxChangeEvent) => heistFilterChange({ ...heistFilters, name: event.target.value?.name })}
                        />
                    </div>
                </div>
            </div>
        </>
    );
};
export default HeistFiltersComponent;
