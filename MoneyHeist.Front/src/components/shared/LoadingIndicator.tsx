import React from 'react';

const LoadingIndicator: React.FC = () => {
    return (
        <div className="k-loading-mask">
            <span className="k-loading-text">Loading</span>
            <div className="k-loading-image"></div>
            <div className="k-loading-color"></div>
        </div>
    );
};

export default LoadingIndicator;
