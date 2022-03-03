import React, { useEffect, useState } from 'react';
import HeistFiltersComponent from './HeistFiltersComponent';
import HeistDetailsComponent from './HeistDetailsComponent';
import { batch, useDispatch, useSelector } from 'react-redux';
import { getHeists } from '../../stores/heist/actions';
import { IHeist } from '../../models/heist';
import { AppState } from '../../stores/rootReducer';

interface IHeistStateProps {
    heists: IHeist[] | undefined;
}
const HeistComponent = () => {
    const dispatch = useDispatch();

    const { heists } = useSelector<AppState, IHeistStateProps>((state: AppState) => {
        return {
            heists: state.heists.heists,
        };
    });

    const [heistsState, setHeistsState] = useState(heists);

    useEffect(() => {
        batch(() => {
            dispatch(getHeists());
        });
    }, [dispatch]);

    useEffect(() => {
        setHeistsState(heists);
    }, [heists]);

    const [heistFilterState, setHeistFiltersState] = useState();

    return (
        <div className="col-12">
            <div className="col-12">
                <HeistFiltersComponent heistFilterChange={setHeistFiltersState} heistFilters={heistFilterState} />
            </div>
            <div className="col-12">
                <HeistDetailsComponent />
            </div>
        </div>
    );
};
export default HeistComponent;
