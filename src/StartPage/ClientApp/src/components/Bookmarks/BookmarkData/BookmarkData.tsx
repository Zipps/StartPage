import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router';
import { connect } from 'react-redux';

import { ApplicationState } from '../../../store';
import * as BookmarksStore from '../../../store/Bookmarks';
import Button from '../../UI/Button/Button';
import Input, { ElementConfig } from '../../UI/Input/Input';
import classes from './BookmarkData.module.css';

type BookmarkProps =
    BookmarksStore.BookmarksState &
    typeof BookmarksStore.actionCreators &
    RouteComponentProps<{}>;

interface Rules {
    required: boolean;
    isUrl?: boolean;
}

interface BookmarkDataConfig {
    id: string;
    elementType: string;
    elementConfig: ElementConfig;
    touched: boolean;    
    value?: string;
    validation: Rules;
    valid: boolean;
    visible: boolean;
}

interface BookmarkDataState {
    bookmarkForm: BookmarkDataConfig[];
    formIsValid: boolean;
    loading: boolean;
}

class BookmarkData extends Component<BookmarkProps> {
    state: BookmarkDataState = {
        bookmarkForm: [
            {
                id: 'id',
                elementType: 'input',
                elementConfig: {
                    type: 'text',
                    placeholder: 'ID'
                },
                value: undefined,
                validation: {
                    required: false
                },
                valid: true,
                touched: false,
                visible: false
            },
            {
                id: 'title',
                elementType: 'input',
                elementConfig: {
                    type: 'text',
                    placeholder: 'Title'
                },
                value: undefined,
                validation: {
                    required: true
                },
                valid: true,
                touched: false,
                visible: true
            },
            {
                id: 'url',
                elementType: 'input',
                elementConfig: {
                    type: 'url',
                    placeholder: 'https://example.com'
                },
                value: undefined,
                validation: {
                    required: true,
                    isUrl: true
                },
                valid: false,
                touched: false,
                visible: true
            },
            { 
                id: 'imageUrl',
                elementType: 'input',
                elementConfig: {
                    type: 'url',
                    placeholder: 'https://example.com/icon.png'
                },
                value: undefined,
                validation: {
                    required: false,
                    isUrl: true
                },
                valid: false,
                touched: false,
                visible: true
            }
        ],
        formIsValid: false,
        loading: false
    }

    public componentDidMount() {
        if (!this.props.loadedBookmark) return;

        const updatedBookmarkForm = [
            ...this.state.bookmarkForm
        ];
        updatedBookmarkForm.map(formElement => {
            switch(formElement.id) {
                case 'id':
                    formElement.value = this.props.loadedBookmark ? this.props.loadedBookmark.id : undefined;
                    break;
                case 'title':
                    formElement.value = this.props.loadedBookmark ? this.props.loadedBookmark.title : undefined;
                    break;
                case 'url':
                    formElement.value = this.props.loadedBookmark ? this.props.loadedBookmark.url : undefined;
                    break;
                case 'imageUrl':
                    formElement.value = this.props.loadedBookmark ? this.props.loadedBookmark.imageUrl : undefined;
                    break;
            }
        });

        this.setState({bookmarkForm: updatedBookmarkForm});
    }

    private bookmarkHandler = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        this.setState({loading: true});
        const bookmark: BookmarksStore.Bookmark =  {};
        this.state.bookmarkForm.map(formElement => {
            switch (formElement.id) {
                case 'title':
                    bookmark.title = formElement.value;
                    break;
                case 'url':
                    bookmark.url = formElement.value;
                    break;
                case 'imageUrl':
                    bookmark.imageUrl = formElement.value;
                    break;
            }
        });
        this.props.saveBookmark(bookmark);
    }

    private inputChangedHandler = (event: React.ChangeEvent<HTMLInputElement>, index: number) => {
        const updatedBookmarkForm = [ 
            ...this.state.bookmarkForm
        ];
        const updatedFormElement = {
            ...updatedBookmarkForm[index]
        };
        updatedFormElement.value = event.target.value;
        updatedFormElement.valid = this.checkValidity(updatedFormElement.value, updatedFormElement.validation);
        updatedFormElement.touched = true;
        updatedBookmarkForm[index] = updatedFormElement;

        let formIsValid = true;
        updatedBookmarkForm.map(formElement => {
            formIsValid = formElement.valid && formIsValid;
        });
        this.setState({bookmarkForm: updatedBookmarkForm, formIsValid: formIsValid});
    }
    
    private checkValidity = (value: string, rules: Rules) => {
        let isValid = true;
        if (!rules) {
            return isValid;
        }

        if (rules.required) {
            isValid = value.trim() !== '' && isValid;
        }

        if (rules.isUrl) {
            const pattern = /[-a-z0-A-Z9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)?/gi;
            isValid = pattern.test(value) && isValid;
        }

        return isValid;
    }

    private closeHandler = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
       event.preventDefault();
       this.props.showBookmark(undefined, true); 
    }

    private deleteBookmarkHandler = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
        event.preventDefault();
        const id = this.props.loadedBookmark ? this.props.loadedBookmark.id || '' : '';
        this.props.deleteBookmark(id);
    }

    public render() {
        let form = (
            <form onSubmit={this.bookmarkHandler}>
                {this.state.bookmarkForm.map((formElement, index) => (
                    <Input
                        key={formElement.id}
                        elementType={formElement.elementType}
                        elementConfig={formElement.elementConfig}
                        value={formElement.value || ''}
                        invalid={!formElement.valid}
                        shouldValidate={formElement.validation != null}
                        touched={formElement.touched}
                        visible={formElement.visible}
                        changedInput={(event: React.ChangeEvent<HTMLInputElement>) => this.inputChangedHandler(event, index)} />
                ))}
                <Button 
                    btnType="Success"
                    disabled={!this.state.formIsValid}>Save</Button>
                <Button
                    btnType="Danger"
                    disabled={!this.props.editMode}
                    clicked={this.deleteBookmarkHandler}>Delete</Button>
                <Button 
                    btnType="Cancel"
                    disabled={false}
                    clicked={this.closeHandler}
                    type="button">Cancel</Button>
            </form>
        );
        return (
            <div className={classes.BookmarkData}>
                <h4>Bookmark</h4>
                {form}
            </div>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.bookmarks,
    BookmarksStore.actionCreators
)(BookmarkData as any);
