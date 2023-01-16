import style from './Registration.module.css';
import { useNavigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';

function Registration() {

    // HOOKS *************************************************************************************

    // Hook for navigation
    let navigate = useNavigate();

    // Hook for the current location
    const location = useLocation();

    // Hook for the copy message
    const [displayCopyMessage, setDisplayCopyMessage] = useState(false);

    // if no state was provided to this page, the user is redirected to the error page
    useEffect(() => {

        if (location === undefined || location.state.playedNumb.length !== 6 || location.state.code.length !== 22) {

            navigate("/error");
        } 

    }, [location, navigate]);   

    // Remove the copy message 3 secs after clicked clicked
    useEffect(() => {

        if(displayCopyMessage){

            setTimeout(() =>
            {
                setDisplayCopyMessage(false);

            }, 3000);
        }

    }, [displayCopyMessage]);

    // FUNCTIONS *********************************************************************************

    const copy = () => {

        navigator.clipboard.writeText(location.state.code);
        setDisplayCopyMessage(true);
    };

    // Function to render the sphere numbers
    const renderSpheres = (playedNumbers) => {

        let renderedNumbers = [];

        playedNumbers.forEach(element => {
            renderedNumbers.push(<div className={style.sphere} key={element}>{element}</div>);
        });

        return renderedNumbers;
    };

    // Function to render the current page
    const renderPage = () => {

        return (
            <section className={style.registration}>
                <h1>Jouer</h1>
                <div>
                    <h2>Votre combinaison</h2>
                    <p>Les numéros ci-dessous sont ceux que vous venez de sélectionner</p>
                    <div className={style.spheresContainer}>
                        {renderSpheres(location.state.playedNumb).map(item => item)}
                    </div>
                </div>
                <div>
                    <h2>Votre code d'accès</h2>
                    <p>Vous trouverez ci-dessous votre code de participation</p>
                    <div id={style["container"]}>
                        <span id="accessCode">{location.state.code}</span>
                        <button onClick={copy}>Copier</button>
                    </div>
                    <span><p style={displayCopyMessage ? {display: 'block'} : {display: 'none'}}>Code enregistré dans le presse papier</p></span>
                    <span>Attention ! Ne perdez pas ce code si vous souhaitez pouvoir consulter vos résultats !</span>
                    <button onClick={() => navigate('/')}>Accueil</button>
                </div>

            </section>
        );
    };

    // RENDERING *********************************************************************************
    
    return renderPage();
    
}

export default Registration;