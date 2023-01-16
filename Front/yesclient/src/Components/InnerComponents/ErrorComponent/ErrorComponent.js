import { useLocation, useNavigate } from 'react-router-dom';
import style from './ErrorComponent.module.css';

function ErrorComponent() {

    // HOOKS *************************************************************************************

    const navigate = useNavigate();
    const location = useLocation();
    
    // FUNCTIONS *********************************************************************************

    // Management of the message to show for the visitor
    const errorMessage = (statusCode) => {

        // Create an empty string container
        let message = '';

        // Give it a value in function of the statuscode this page will receive
        switch (statusCode) {

            case '401':
                message = "Vous n'êtes pas autorisé à acceder à cette resource";
                break;

            case '403':
                message = "Action non autorisée";
                break;

            case '404':
                message = "Resource introuvable";
                break;

            default:
                message = "Erreur avec le serveur... Nous vous invitons à rééssayer plus tard";

        };

        return message;

    };

    // RENDERING *********************************************************************************

    return(
        <section className={style.errorpage}>
            <h1>Oups...</h1>
            <div>
                <h3>Une erreur s'est produite...</h3>
                <p>{ errorMessage(location.state.status) } (Code { location.state.status }).</p>
                <button onClick={() => navigate('/')}>Accueil</button>
            </div>
        </section>
    );
}

export default ErrorComponent;