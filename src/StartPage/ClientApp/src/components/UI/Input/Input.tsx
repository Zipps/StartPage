import React from 'react';

import classes from './Input.module.css';

export interface ElementConfig {
    type: string;
    placeholder?: string;
    options?: HTMLOptionElement[]
}

interface InputProps {
    changedInput?: (event: React.ChangeEvent<HTMLInputElement>) => void;
    changedSelect?: (event: React.ChangeEvent<HTMLSelectElement>) => void;
    changedTextarea?: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
    elementConfig: ElementConfig;
    elementType: string;
    invalid: boolean;
    label?: string;
    shouldValidate: boolean;
    touched: boolean;
    value: string;
    visible: boolean;
}

const input = ( props: InputProps ) => {
    let inputElement = null;
    const inputClasses = [classes.InputElement];

    if (props.invalid && props.shouldValidate && props.touched) {
        inputClasses.push(classes.Invalid);
    }

    switch ( props.elementType ) {
        case ( 'input' ):
            inputElement = <input
                className={inputClasses.join(' ')}
                {...props.elementConfig}
                value={props.value}
                onChange={props.changedInput} />;
            break;
        case ( 'textarea' ):
            inputElement = <textarea
                className={inputClasses.join(' ')}
                {...props.elementConfig}
                value={props.value}
                onChange={props.changedTextarea} />;
            break;
        case ( 'select' ):
            inputElement = (
                <select
                    className={inputClasses.join(' ')}
                    value={props.value}
                    onChange={props.changedSelect}>
                    {props.elementConfig.options != null
                     ? props.elementConfig.options.map(option => (
                        <option key={option.value} value={option.value}>
                            {option.label}
                        </option>
                        )) : null}
                </select>
            );
            break;
        default:
            inputElement = <input
                className={inputClasses.join(' ')}
                {...props.elementConfig}
                value={props.value}
                onChange={props.changedInput} />;
    }

    const divClasses = [classes.Input];
    if (!props.visible) {
        divClasses.push(classes.Hidden);
    }
    return (
        <div className={divClasses.join(' ')}>
            <label className={classes.Label}>{props.label}</label>
            {inputElement}
        </div>
    );

};

export default input;