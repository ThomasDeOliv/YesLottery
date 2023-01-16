import style from './Play.module.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Play(props) {

    // HOOKS *************************************************************************************

    // Hook for navigate on the app
    let navigate = useNavigate();

    // Hook for the state of the erease button
    const [disableDelete, setDisableDelete] = useState(true);

    // Hook for the state of the validate button
    const [disableValidate, setDisableValidate] = useState(true);

    // Hook for the sentence to display to the user
    const [counterSentence, setCounterSentence] = useState('Choisissez 6 numéros dans la liste ci-dessous');

    // Hook to contain the selected elements
    const [selection, setSelection] = useState([]);

    // Effect for unlock the reset button, the validate button and the information sentence
    useEffect(() => {
        
        let sentence = '';

        switch(selection.length) {

            case 6 :
                sentence = 'Validez votre sélection pour passer à la suite';
                setDisableValidate(false);
                setDisableDelete(false);
                break;
            case 5 :
                sentence = 'Choisissez encore 1 numéro dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(false);
                break;
            case 4 :
                sentence = 'Choisissez 2 numéros dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(false);
                break;
            case 3 :
                sentence = 'Choisissez 3 numéros dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(false);
                break;
            case 2 :
                sentence = 'Choisissez 4 numéros dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(false);
                break;
            case 1 :
                sentence = 'Choisissez 5 numéros dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(false);
                break;
            default :
                sentence = 'Choisissez 6 numéros dans la liste ci-dessous';
                setDisableValidate(true);
                setDisableDelete(true);
                break;
        };

        setCounterSentence(sentence);

    }, [selection]);

    // If a draw is closed to participation, we redirect user to homepage
    useEffect(() => {

        if (props.currentDrawClosed)
        {
            props.setClosedModal(true);
            navigate('/');
        }

    }, [props, navigate])

    // FUNCTIONS *********************************************************************************

    // Function called when pressing validate button
    const pushButton = (e) => {

        // Getting the value inside button
        const innerValue = e.target.innerHTML;

        // To select a number : maximum of 6 distincts values
        if(!selection.includes(innerValue) && selection.length < 6) {

            // Adding selection to collection
            let newSelection = [...selection];
            newSelection.push(innerValue);
            setSelection(newSelection);

            // Change the style of the button associated with the selected value
            const element = e.target;
            element.style.backgroundColor = '#FFFFFF';
            element.style.color = '#2D3047';
        }

        // If the value is already in the selection, w
        if(selection.includes(innerValue)) {

            // Remove selection to collection
            let newSelection = [...selection];
            const index = newSelection.indexOf(innerValue);
            newSelection.splice(index, 1);
            setSelection(newSelection);

            // Reset the style of the associated button
            const element = e.target;
            element.style.backgroundColor = '#2D3047';
            element.style.color = '#FFFFFF';
        }
    };

    // Function called when pressing reset button
    const reset = () => {

        setSelection([]);

        const elements = document.querySelectorAll(`[class=${style.numberButton}]`);
        elements.forEach(element => 
            {
                element.style.backgroundColor = '#2D3047';
                element.style.color = '#FFFFFF';
            });
    };

    // Function called when pressing validate button
    const validateTicket = async () => {

        // URL to contact
        const url = process.env.REACT_APP_API_ENTRYPOINT_TICKETS;

        // Options
        const options = { 

            method: 'POST', 
            headers: { 
                "Content-type": "application/json",
                "Access-Control-Allow-Origin": process.env.REACT_APP_ORIGIN_URL
            },
            body: JSON.stringify(selection)
        };

        try {

            let data = await fetch(url, options)
                .then(response => {

                    if (!response.ok) {

                        throw new Error(response.status);
                    }

                    return response.text();
                });

            setSelection(selection.sort((a, b) => a - b));
            navigate('/registration', { state: { code: data, playedNumb: selection } });

        } catch (error) {

            navigate('/error', { state: { status: error } });
        }
    };

    // Function to render the keyboard
    const keyboardRender = (max) => {

        return Array.from({ length: max }, (_, num) => <button key={num + 1} className={style.numberButton} onClick={(e) => pushButton(e)}>{num + 1}</button>);

    }
    
    // RENDERING *********************************************************************************

    return (
        <section className={style.playsection}>
            <h2>Jouer</h2>
            <div>
                <h3>Sélectionnez vos numéros</h3>
                <span>{counterSentence}</span>
                <div>
                    { keyboardRender(49) }
                </div>
                <div>
                    <button onClick={reset} disabled={disableDelete}>Réinitialiser</button>
                    <button onClick={validateTicket} disabled={disableValidate}>Valider</button>
                </div>
            </div>
        </section>
    );
}

export default Play;